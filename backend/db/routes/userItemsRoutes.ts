import express from "express";
import { updateCoins, updateRewards } from "../controllers/userItemsController";

const userItemsRouter = express.Router();

userItemsRouter.put("/update-coins", updateCoins);

userItemsRouter.put("/update-rewards", updateRewards);

export default userItemsRouter;