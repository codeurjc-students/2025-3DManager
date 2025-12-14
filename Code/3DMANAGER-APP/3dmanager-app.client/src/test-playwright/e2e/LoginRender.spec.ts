import { test, expect } from '@playwright/test'


test('LoginRender', async ({ page }) => {
  await page.goto('/');
  await expect(page.locator('h2')).toHaveText('Inicio sesión');
  await expect(page.locator('text=No conectado')).toBeVisible();
});