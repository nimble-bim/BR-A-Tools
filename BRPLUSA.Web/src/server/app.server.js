import express from 'express';
import path from 'path';
import template from './templates/main';
import attachServer from './config/socket.config';

const app = express();
const port = 4422;

app.use(express.static(path.resolve(__dirname, '../dist')));
app.get('*', (req, res, next) => {
  res.send(template);
});

const server = app.listen(port, () => {
  console.log(`Server is listening on port: ${port}`);
});

attachServer(server);
