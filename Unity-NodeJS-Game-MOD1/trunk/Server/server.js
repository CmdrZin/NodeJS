var port = process.env.PORT || 3000;
var io = require('socket.io')(port);
var shortId = require('shortid');

var players = [];

var playerSpeed = 3;

console.log("server started on port " + port);

/*
 * A 'socket' is established for each Client that connects.
 * So in essence, each io.on() is unique for each socket.
 */
io.on('connection', function (socket) {

    var thisPlayerId = shortId.generate();
    var player = {
        id: thisPlayerId,
        destination: {
            x: 0,
            y: 0
        },
        lastPosition: {
            x: 0,
            y: 0
        },
        lastMoveTime: 0,
        color: 0
    };
    players[thisPlayerId] = player;

    console.log("client connected, id = ", thisPlayerId);

    socket.emit('register', {id: thisPlayerId});
    // Stop here until the color message returns with the player's color.
    // COntinue setup with the 'color' Event.

    /* These are unique handlers for this Socket connection. Each Socket will 
     * be unique, which is why thisPlayerId is maintained. It's for this socket.
     */
    socket.on('move', function (data) {
        data.id = thisPlayerId;
        console.log('client moved', JSON.stringify(data));
        /* DEBUG CODE
         console.log('socket ', socket.id);  // shows that each socket has it's own io.on().
         */
        player.destination.x = data.d.x;
        player.destination.y = data.d.y;

        var elapsedTime = Date.now() - player.lastMoveTime;

        var travelDistanceLimit = elapsedTime * playerSpeed / 1000;

        var requestedDistanceTraveled = lineDistance(player.lastPosition, data.c);

        player.lastMoveTime = Date.now();

        player.lastPosition = data.c;

        delete data.c;

        data.x = data.d.x;
        data.y = data.d.y;

        delete data.d;

        socket.broadcast.emit('move', data);

        console.log('Player moved ', JSON.stringify(data));		// DEBUG CODE
    });

    socket.on('follow', function (data) {
        data.id = thisPlayerId;
        console.log("follow request: ", data);
        socket.broadcast.emit('follow', data);
    });

    socket.on('updatePosition', function (data) {
        console.log("update position: ", data);
        data.id = thisPlayerId;
        socket.broadcast.emit('updatePosition', data);
    });

    socket.on('attack', function (data) {
        console.log("attack request: ", data);
        data.id = thisPlayerId;
        io.emit('attack', data);
    });

    // Update the player object for this socket AND update the player list.
    socket.on('color', function (data) {
        player.color = data.color;        // Record the player's color
        players[thisPlayerId] = player;   // Update the list of players.
        console.log("color set to: ", player.color);
        // Finish the new player process.
        // Tell all of the other players to spawn this player.
        socket.broadcast.emit('spawn', {id: thisPlayerId, color: player.color});
        // Tell all players to send an updatePosition message to the server.
        socket.broadcast.emit('requestPosition');
        // which will then broadcast updates on everyone position.
        
        // Tell the new Client to spawn all of the other players.
        for (var playerId in players) {
            if (playerId == thisPlayerId)
                continue;
            socket.emit('spawn', players[playerId]);
        }
    });

    socket.on('disconnect', function () {
        console.log('client disconected');
        delete players[thisPlayerId];
        socket.broadcast.emit('disconnected', {id: thisPlayerId});
    });
});

function lineDistance(vectorA, vectorB) {
    var xs = 0;
    var ys = 0;

    xs = vectorB.x - vectorA.x;
    xs = xs * xs;

    ys = vectorB.y - vectorA.y;
    ys = ys * ys;

    return Math.sqrt(xs + ys);
}