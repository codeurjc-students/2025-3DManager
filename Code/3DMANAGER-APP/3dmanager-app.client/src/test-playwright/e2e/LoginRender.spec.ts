import { test, expect } from '@playwright/test'


test('LoginRender', async ({ page }) => {
  await page.goto('/');

  // Verifica que se muestra el título de la pantalla de login
  await expect(page.locator('h2')).toHaveText('Inicio sesión');

  // Verifica que aparece el estado "No conectado"
  await expect(page.locator('text=No conectado')).toBeVisible();
});