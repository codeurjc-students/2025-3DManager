import { vi } from 'vitest'
import '@testing-library/jest-dom'

//Mock del módulo printerService 
vi.mock('./api/printerService', () => ({
    __esModule: true,
    getPrinterList: vi.fn(() => Promise.resolve({ data: [] })),
}))

//Mock global de axios para cortar cualquier import accidental
vi.mock('axios', () => {
    return {
        default: {
            create: () => ({
                get: vi.fn(() => Promise.resolve({ data: {} })),
                post: vi.fn(() => Promise.resolve({ data: {} })),
                put: vi.fn(() => Promise.resolve({ data: {} })),
                delete: vi.fn(() => Promise.resolve({ data: {} })),
            }),
        },
    }
})