import express from "express";
import { register, login, getUser } from "../controllers/userController";

const userRouter = express.Router();

userRouter.post("/register", register);

userRouter.post("/login", login);

export default userRouter;