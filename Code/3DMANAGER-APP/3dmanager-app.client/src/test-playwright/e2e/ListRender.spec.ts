import { test, expect } from '@playwright/test';

test.describe('ListRenderE2E', () => {

    test('FilamentListPageRender', async ({ page }) => {
        await page.goto('dashboard/lists/filaments');

        await page.waitForSelector('#listContainer');

        const headers = page.locator('table thead th');
        await expect(headers.nth(0)).toHaveText('Nombre');
        await expect(headers.nth(1)).toHaveText('Estado');
        await expect(headers.nth(2)).toHaveText('Filamento restante');
        await expect(headers.nth(3)).toHaveText('Coste filamento');
        await expect(headers.nth(4)).toHaveText('Detalle');

    
        const rows = page.locator('table tbody tr');
        await expect(rows.first()).toBeVisible({ timeout: 10000 });
        const rowCount = await rows.count();
        expect(rowCount).toBeGreaterThan(0);
 
        if (rowCount > 0) {
            await expect(rows.first().locator('td').nth(0)).toBeVisible();
            await expect(rows.first().locator('td').nth(1)).toBeVisible();
        }
    });

    test('PrintListPageRender', async ({ page }) => {
        
        await page.goto('dashboard/lists/prints');
        await page.waitForSelector('#listContainer');
        
        const headers = page.locator('table thead th');
        await expect(headers.nth(0)).toHaveText('Nombre');
        await expect(headers.nth(1)).toHaveText('Usuario');
        await expect(headers.nth(2)).toHaveText('Fecha impresión');
        await expect(headers.nth(3)).toHaveText('Tiempo impresion');
        await expect(headers.nth(4)).toHaveText('Filamento consumido');
        await expect(headers.nth(5)).toHaveText('Detalle');

        
        const rows = page.locator('table tbody tr');
        await expect(rows.first()).toBeVisible({ timeout: 10000 });
        const rowCount = await rows.count();
        expect(rowCount).toBeGreaterThan(0);

        if (rowCount > 0) {
            await expect(rows.first().locator('td').nth(0)).toBeVisible(); 
            await expect(rows.first().locator('td').nth(1)).toBeVisible();
        }
    });

    test('UserListPageRender', async ({ page }) => {
        
        await page.goto('dashboard/lists/users');

        await page.waitForSelector('#listContainer');
        
        const headers = page.locator('table thead th');
        await expect(headers.nth(0)).toHaveText('Nombre');
        await expect(headers.nth(1)).toHaveText('Horas mes actual');
        await expect(headers.nth(2)).toHaveText('Piezas mes actual');
        await expect(headers.nth(3)).toHaveText('Detalle');

        
        const rows = page.locator('table tbody tr');
        await expect(rows.first()).toBeVisible({ timeout: 10000 });
        const rowCount = await rows.count();
        expect(rowCount).toBeGreaterThan(0);

        if (rowCount > 0) {
            await expect(rows.first().locator('td').nth(0)).toBeVisible(); 
            await expect(rows.first().locator('td').nth(1)).toBeVisible(); 
        }
    });
});