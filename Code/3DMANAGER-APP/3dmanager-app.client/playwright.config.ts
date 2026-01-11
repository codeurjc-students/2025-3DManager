import { defineConfig, devices } from '@playwright/test';

const isCI = !!process.env.CI;
const port = isCI ? 3001 : 3000;

export default defineConfig({
    fullyParallel: false,       
    workers: 1,  
    testDir: './src/test-playwright/e2e',
    timeout: 60 * 1000,
    expect: {
        timeout: 5000
    },
    reporter: [['list'], ['html', { open: 'never' }]],
    globalSetup: './src/test-playwright/globalSetup',
    use: {
        baseURL: `${isCI ? 'http' : 'https'}://localhost:${port}`,
        headless: true,
        viewport: { width: 1280, height: 720 },
        ignoreHTTPSErrors: true,
        storageState: 'playwright/.auth/state.json'
    },
    projects: [
        { name: 'chromium', use: { ...devices['Desktop Chrome'] } },
        { name: 'firefox', use: { ...devices['Desktop Firefox'] } },
        { name: 'webkit', use: { ...devices['Desktop Safari'] } },
    ],
})
