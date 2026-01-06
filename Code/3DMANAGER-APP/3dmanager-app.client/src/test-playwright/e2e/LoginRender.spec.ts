import { test, expect } from '@playwright/test'
import { loginIfCI } from './AuthHelperTest';


test('LoginRender', async ({ page }) => {
  await page.goto('/');
  await expect(page.locator('h2')).toHaveText('Inicio sesión');
  await expect(page.locator('text=No conectado')).toBeVisible();
  await loginIfCI(page);
});