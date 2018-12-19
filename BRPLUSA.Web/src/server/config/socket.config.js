import socketIO from 'socket.io';

let socketServer;
let webSocket;

const attachServer = (server) => {
  socketServer = socketIO.listen(server);
  socketServer.on('connection', handleIO);
};

const handleIO = (socket) => {
  webSocket = socket;
  handleInitialConnection(webSocket);

  webSocket.on('disconnect', handleDisconnection);

  webSocket.on('AUTHORIZATION_REQUESTED', handleAuthorization);

  webSocket.on('COMMAND_BACKUP', handleBackup);

  webSocket.on('ROOM_CREATE', createRoom);
};

const handleInitialConnection = (socket) => {
  const clientId = socket.client.id;

  console.log('');
  console.log('New client connected!');
  console.log(`Client Id:   ${clientId}`);
};

const handleDisconnection = (id) => {
  console.log(`${id} disconnected`);
};

const createRoom = (id) => {
  console.log(`Attempting to create room for ${id}`);
};

const handleBackup = (id) => {
  console.log('Handling backup command from Revit', id);
  socketServer.sockets.emit('BACKUP_REQUESTED', id);
};

const handleAuthorization = (id) => {
  console.log(`Authorization requested from ${id}`);
  // do some other auth stuff?

  webSocket.join(id);
  socketServer.in(id).emit('AUTHORIZATION_GRANTED', id);
};

export default attachServer;
