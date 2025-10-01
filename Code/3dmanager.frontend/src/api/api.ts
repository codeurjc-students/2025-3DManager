import { ApiClient } from "./generated/apiClient";

export const apiClient = new ApiClient(
  process.env.REACT_APP_API_URL || "https://localhost:7024"
);

