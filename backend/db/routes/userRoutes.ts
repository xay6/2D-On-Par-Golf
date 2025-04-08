import express from "express";
import { register, login, getUser, deleteUser } from "../controllers/userController";

const userRouter = express.Router();

userRouter.post("/register", register);

userRouter.post("/login", login);

userRouter.get("/get-user", getUser);

userRouter.delete("/delete-user", deleteUser);

export default userRouter;