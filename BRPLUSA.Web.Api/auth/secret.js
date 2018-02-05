'use strict';

module.exports = {
  env: process.env.NODE_ENV || 'dev',
  port: process.env.POR || 7855,
  db: {
    uri: 'mongodb://Prestonsmith2:vdDw6kHYa41j2gqv@poopypants-shard-00-00-ysi1e.mongodb.net:27017,poopypants-shard-00-01-ysi1e.mongodb.net:27017,poopypants-shard-00-02-ysi1e.mongodb.net:27017/test?ssl=true&replicaSet=PoopyPants-shard-0&authSource=admin',
    auth: "vdDw6kHYa41j2gqv"
  },
}