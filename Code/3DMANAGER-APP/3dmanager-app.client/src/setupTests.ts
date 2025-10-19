import '@testing-library/jest-dom'

vi.mock('./src/api/apiClient', () => {
    return {
        default: {
            get: vi.fn(() => Promise.resolve({ data: [] })),
            post: vi.fn(() => Promise.resolve({ data: {} })),
            put: vi.fn(() => Promise.resolve({ data: {} })),
            delete: vi.fn(() => Promise.resolve({ data: {} })),
            create: vi.fn(() => ({}))
        }
    }
})