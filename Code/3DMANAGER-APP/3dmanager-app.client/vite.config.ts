import { fileURLToPath, URL } from 'node:url';

import { defineConfig } from 'vitest/config'
import plugin from '@vitejs/plugin-react';
import fs from 'fs';
import path from 'path';
import child_process from 'child_process';
import { env } from 'process';

const baseFolder =
    env.APPDATA !== undefined && env.APPDATA !== ''
        ? `${env.APPDATA}/ASP.NET/https`
        : `${env.HOME}/.aspnet/https`;

const certificateName = "3dmanager-app.client";
const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

if (!fs.existsSync(baseFolder)) {
    fs.mkdirSync(baseFolder, { recursive: true });
}

if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
    if (0 !== child_process.spawnSync('dotnet', [
        'dev-certs',
        'https',
        '--export-path',
        certFilePath,
        '--format',
        'Pem',
        '--no-password',
    ], { stdio: 'inherit', }).status) {
        throw new Error("Could not create certificate.");
    }
}

const isCI = process.env.ASPNETCORE_ENVIRONMENT === 'CI';

const target = isCI
    ? 'http://localhost:5000'
    : env.ASPNETCORE_HTTPS_PORT
        ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
        : 'https://localhost:443';

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [plugin()],
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        }
    },
    server: isCI
        ? { //CI mode
            port: 3001,
            https: undefined,
            proxy: {
                '^/api': {
                    target,
                    changeOrigin: true
                }
            }
        }
        : {
            proxy: {
                '^/api': {
                    target,
                    secure: false,
                    changeOrigin: true,
                }
            },
            //port: parseInt(env.DEV_SERVER_PORT || '53242'),
            port: 3000,
            https: {
                key: fs.readFileSync(keyFilePath),
                cert: fs.readFileSync(certFilePath),
            }

        },
});
