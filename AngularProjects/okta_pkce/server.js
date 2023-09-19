const http = require('http');
const app = require('./app');
const server = http.createServer(app);
const port = process.env.PORT || 3000;
// set configuration for the express environment
app.set('port', port);
server.listen(port);