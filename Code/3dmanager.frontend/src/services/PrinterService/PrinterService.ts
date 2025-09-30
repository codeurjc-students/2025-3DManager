import api from "../../api/axiosConfig";
import { PrinterObject } from "../../models/Printer/PrinterObject";

export async function getPrinterList(): Promise<PrinterObject[]> {
  const response = await api.get<PrinterObject[]>("/Printer/GetPrinterList"); 
  return response.data;
}
