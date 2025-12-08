import { vi } from 'vitest'
import '@testing-library/jest-dom'

vi.mock('./api/printerService', () => ({
    __esModule: true,
    getPrinterList: vi.fn(() => Promise.resolve({ data: [] })),
}))

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