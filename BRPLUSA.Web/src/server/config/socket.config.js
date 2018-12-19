import socketIO from 'socket.io';
import server from './server.config';
const io = socketIO.listen(server);

const handleConnection = socket => {
  const clientId = socket.client.id;

  console.log('');
  console.log('New client connected!');
  console.log(`Client Id:   ${clientId}`);

  socket.broadcast.emit('AUTHORIZATION_REQUESTED', clientId); // try using this
};

const handleDisconnect = () => {
  console.log('user disconnected');
};

const handleBackup = id => {
  console.log('Handling backup command from Revit', id);
  io.sockets.emit('back up', id);
};

const handleAuthorization = (id, socket) => {
  console.log(id);
  // do some other auth stuff?

  socket.join(id);
  socket.broadcast.to(id).emit('AUTHORIZATION_GRANTED', id);
};

const handleConfirmation = id => {
  // confirm auth?
  console.log(`The client: ${id} has been authorized`);
};

const configureSockets = () => {
  io.on('connection', handleConnection);
  io.on('disconnect', handleDisconnect);

  io.on('COMMAND_BACKUP', handleBackup);
  io.on('AUTHORIZATION_REQUESTED', handleAuthorization);
  io.on('AUTHORIZATION_GRANTED', handleConfirmation);
};

export default configureSockets;
