import { apiClient } from "../../api/api";

export const getPrinters = async () => {
  return await apiClient.getPrinterList(); // devuelve los datos tipados automáticamente
};
