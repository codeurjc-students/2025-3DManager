import { test, expect } from '@playwright/test'

test('listar impresoras correctamente', async ({ page }) => {
    await page.goto('/') 

    await expect(page.locator('h2')).toHaveText('Impresoras:')

    await expect(page.locator('li')).toHaveCount(4)
    await expect(page.locator('li').first()).toHaveText('Impresora prueba 1')
})
