const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:59571';

const jwtWebApi = "https://localhost:7258";

const PROXY_CONFIG = [
  {
    context: [
      "/weatherforecast",
   ],
    proxyTimeout: 10000,
    target: jwtWebApi, // target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    },
    "changeOrigin": false
  }
]

module.exports = PROXY_CONFIG;
