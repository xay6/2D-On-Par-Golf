import express from "express";
import { register, login, getUser, deleteUser, logout } from "../controllers/userController";

const userRouter = express.Router();

userRouter.post("/register", register);

userRouter.post("/login", login);

userRouter.get("/get-user", getUser);

userRouter.delete("/delete-user", deleteUser);

userRouter.post("/logout", logout)

export default userRouter;