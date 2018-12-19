import React from 'react';
import Express from 'express';
import CORS from 'cors';
import { renderToString } from 'react-dom/server';
import App from '../../shared/app.shared';

const app = Express();
const port = process.env.PORT || 3000;

app.use(CORS());

// We're going to serve up the public
// folder since that's where our
// client bundle.js file will end up.
app.use(Express.static('public'));

app.get('*', (req, res, next) => {
  //   const markup = renderToString(<App />);
  //   const template = `
  //   <!DOCTYPE html>
  //   <html>
  //     <head>
  //       <title>SSR with RR</title>
  //       <script src="/client.js" defer></script>
  //     </head>

  //     <body>
  //       <div id="app">${markup}</div>
  //     </body>
  //   </html>
  // `;
  //   res.send(template);
  res.send('Hello');
});

app.listen(port, () => {
  console.log(`Server is listening on port: ${port}`);
});

export default app;
