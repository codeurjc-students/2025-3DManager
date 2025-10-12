import { test, expect } from '@playwright/test'

test('listar impresoras correctamente', async ({ page }) => {
    await page.goto('/') // abre la app

    // Verifica que el título esté
    await expect(page.locator('h2')).toHaveText('Listado de Impresoras')

    // Verifica que se cargan las impresoras de la API
    await expect(page.locator('li')).toHaveCount(3)
    await expect(page.locator('li').first()).toHaveText('Impresora prueba 1')
})
