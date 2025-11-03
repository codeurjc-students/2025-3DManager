import { render, screen, waitFor } from '@testing-library/react'
import PrinterList from '../../components/PrinterList'
import * as printerService from '../../api/printerService'

// Mock de la API
const mockPrinters = [
    { printerName: 'Impresora prueba test 1' },
    { printerName: 'Impresora prueba test 2' },
]

vi.spyOn(printerService, 'getPrinterList').mockResolvedValue({ data: mockPrinters })

describe('PrinterList Integration', () => {
    it('renderiza la lista de impresoras correctamente', async () => {
        render(<PrinterList />)

        expect(screen.getByText(/Cargando impresoras/i)).toBeInTheDocument()

        await waitFor(() => {
            mockPrinters.forEach((printer) => {
                expect(screen.getByText(printer.printerName)).toBeInTheDocument()
            })
        })
    })

    it('muestra mensaje de error si falla la API', async () => {
        vi.spyOn(printerService, 'getPrinterList').mockRejectedValueOnce(new Error('API Fallida'))

        render(<PrinterList />)

        await waitFor(() => {
            expect(screen.getByText(/Error al cargar las impresoras/i)).toBeInTheDocument()
        })
    })
})
