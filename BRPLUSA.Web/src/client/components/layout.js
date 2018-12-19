import React from 'react';
import openSocket from 'socket.io-client';
// import './Layout.css';

const divStyle = {
  backgroundColor: 'red'
};

export default class Layout extends React.Component {
  constructor() {
    super();
    this.state = {
      title: 'Even smaller functions built',
      socket: null,
      socketAddress: 'http://localhost:4422'
    };
    this.connectSocket(this.state.socketAddress);
  }

  connectSocket(url) {
    this.state.socket = openSocket(url);
    this.state.socket.on('connect', data => {
      const id = this.state.socket.id;
      console.log(`${id} has been connected on the client side!`);
      this.state.socket.emit('AUTHORIZATION_REQUESTED', this.state.socket.id);
    });
    this.state.socket.on('AUTHORIZATION_GRANTED', data => {
      console.log('Authorization was granted by the server');
      console.log(`${data} was approved to working in this area`);
    });
  }

  backup() {
    this.state.socket.emit('COMMAND_BACKUP', this.state.socket.id);
  }

  render() {
    return (
      <div style={divStyle}>
        <h1>{this.state.title}</h1>
        <button onClick={() => this.backup()}>Hit me!</button>
      </div>
    );
  }
}
