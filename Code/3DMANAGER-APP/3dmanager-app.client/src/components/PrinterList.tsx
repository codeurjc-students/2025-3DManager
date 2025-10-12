import React, { useEffect, useState } from 'react'
import { getPrinterList } from '../api/printerService'
import type { PrinterObject } from '../models/printer/PrinterObject'

const PrinterList: React.FC = () => {
    const [printers, setPrinters] = useState<PrinterObject[]>([])
    const [loading, setLoading] = useState(true)
    const [error, setError] = useState<string | null>(null)

    useEffect(() => {
        const fetchPrinters = async () => {
            try {
                const result = await getPrinterList()
                setPrinters(result.data ?? [])
            } catch (err) {
                console.error(err)
                setError('Error al cargar las impresoras')
            } finally {
                setLoading(false)
            }
        }

        fetchPrinters()
    }, [])

    return (
        <div style= {{ padding: '1rem' }}>
            <h2>Listado de Impresoras </h2>

            { loading && <p>Cargando impresoras...</p> }
            { error && <p style={ { color: 'red' } }> { error } </p> }

            <ul>
            {
                printers.map((p, i) => (
                    <li key= { i } > { p.printerName } </li>
                ))
            }
            </ul>
        </div>
    ) 
}

export default PrinterList
