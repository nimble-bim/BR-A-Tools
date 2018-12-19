import http from 'http';
import app from './express.config';
import { onError, onListening, normalizePort } from './events.config';
const port = normalizePort(process.env.PORT || '3000');
const server = http.createServer(app);

app.set('port', port);
server.listen(port);

server.on('error', onError);
server.on('listening', () => {
  onListening(server);
});

export default server;
