import express, { Request, Response } from 'express';

import connectDB from "./db/connection";
import * as dotenv from "dotenv";
dotenv.config({ path: __dirname + '/.env' });
    
const port = process.env.PORT;

const app = express();

app.get('/', (req: Request, res: Response) => {
  res.send('Server running.');
});

connectDB();

app.listen(port, () => {
  console.log(`Server is running on port ${port}`);
});
