import { createClient } from 'then-redis'

const db = createClient({
    host: 'localhost',
    port: 6379,
    password: 'password'
  })

  db.get('PortfolioMock').then(function (value) {
    console.log(value);
  })