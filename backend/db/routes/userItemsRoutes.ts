import express from "express";
import { getCoins, getRewards, updateCoins, updateRewards } from "../controllers/userItemsController";

const userItemsRouter = express.Router();

userItemsRouter.put("/update-coins", updateCoins);

userItemsRouter.put("/update-rewards", updateRewards);

userItemsRouter.get("/get-rewards", getRewards);

userItemsRouter.get("/get-coins", getCoins);

export default userItemsRouter;