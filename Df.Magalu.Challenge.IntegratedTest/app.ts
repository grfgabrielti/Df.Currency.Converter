const fs = require('fs');
let rawdata = fs.readFileSync('product.json');
var value = new Buffer(rawdata).toString();

var id = value[id];
console.log('id')
console.log(id)
console.log('product');
console.log(value);
console.log(typeof(value));