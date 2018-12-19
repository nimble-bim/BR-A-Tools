const path = require('path');
const nodeExternals = require('webpack-node-externals');

const serverConfig = {
  mode: 'development',
  target: 'node',
  node: {
    // allows dirname to be used correctly
    __dirname: false,
  },
  entry: path.resolve(__dirname, 'src/server/app.server.js'),
  output: {
    path: path.resolve(__dirname, 'dist'),
    filename: 'server.js',
    publicPath: '/',
  },
  module: {
    rules: [
      {
        test: /\.js$/,
        loader: 'babel-loader',
        query: {
          presets: ['@babel/preset-env', '@babel/preset-react'],
        },
      },
    ],
  },
  stats: {
    colors: true,
  },
  devtool: 'source-map',
  externals: [nodeExternals()],
};

const clientConfig = {
  mode: 'development',
  node: {
    // allows dirname to be used correctly
    __dirname: false,
  },
  entry: path.resolve(__dirname, 'src/client/app.client.js'),
  output: {
    path: path.resolve(__dirname, 'dist'),
    filename: 'client.js',
    publicPath: '/',
  },
  module: {
    rules: [
      {
        test: /\.js$/,
        loader: 'babel-loader',
        query: {
          presets: ['@babel/preset-env', '@babel/preset-react'],
        },
      },
    ],
  },
  stats: {
    colors: true,
  },
  devtool: 'source-map',
};

module.exports = [clientConfig, serverConfig];
