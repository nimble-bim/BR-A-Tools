// const debug = require()
/**
 * Normalize a port into a number, string, or false.
 */

function normalizePort(val) {
    const port = parseInt(val, 10);
    /* eslint-disable no-restricted-globals */
    if (isNaN(port)) {
      // named pipe
      return val;
    }
  
    if (port >= 0) {
      // port number
      return port;
    }
  
    return false;
  }
  /* eslint-enable no-restricted-globals */
  
  /**
   * Event listener for HTTP server "error" event.
   */
  
  function onError(error, port) {
    if (error.syscall !== 'listen') {
      throw error;
    }
  
    const bind = typeof port === 'string' ? `Pipe ${port}` : `Port ${port}`;
  
    // handle specific listen errors with friendly messages
    /* eslint-disable indent */
    /* eslint-disable no-console */
    switch (error.code) {
      case 'EACCES':
        console.error(`${bind} requires elevated privileges`);
        process.exit(1);
        break;
      case 'EADDRINUSE':
        console.error(`${bind} is already in use`);
        process.exit(1);
        break;
      default:
        throw error;
    }
  }
  /* eslint-enable indent */
  
  /**
   * Event listener for HTTP server "listening" event.
   */
  
  function onListening(server) {
    const addr = server.address();
    const bind = typeof addr === 'string' ? `pipe ${addr}` : `port ${addr.port}`;
    // debug('Listening on ' + bind);
    console.log(`Listening on ${bind}`);
  }
  
  module.exports = {
    onListening,
    onError,
    normalizePort,
  };
  