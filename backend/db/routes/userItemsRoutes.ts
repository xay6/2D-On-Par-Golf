import express from "express";
import { updateCoins } from "../controllers/userItemsController";

const userItemsRouter = express.Router();

userItemsRouter.put("/update-coins", updateCoins);

export default userItemsRouter;