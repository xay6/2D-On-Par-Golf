import express, { Request, Response } from 'express';

import connectDB from "./db/connection";
import userRoutes from './db/routes/userRoutes';
import * as dotenv from "dotenv";
dotenv.config();
    
const port = process.env.PORT;

const app = express();
app.use(express.json());

app.get('/', (req: Request, res: Response) => {
  res.send('Server running.');
});

app.use("/api", userRoutes);

connectDB();

app.listen(port, () => {
  console.log(`Server is running on port ${port}`);
});
