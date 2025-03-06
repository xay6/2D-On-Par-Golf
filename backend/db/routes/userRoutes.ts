import express from "express";
import { register, login } from "../controllers/userController";

const userRouter = express.Router();

userRouter.post("/users/register", register);

userRouter.post("/users/login", login);

export default userRouter;