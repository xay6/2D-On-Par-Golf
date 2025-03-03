import express, { Request, Response } from 'express';
import cors from 'cors';
    
const port = 3000;

const app = express();
app.use(cors({ credentials: true, origin: true }));

app.get('/', (req: Request, res: Response) => {
  res.send('Hello, World!');
});

app.listen(port, () => {
  console.log(`Server is running on port ${port}`);
});
