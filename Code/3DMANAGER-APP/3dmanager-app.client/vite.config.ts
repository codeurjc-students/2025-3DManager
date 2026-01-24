import { fileURLToPath, URL } from 'node:url'
import { defineConfig } from 'vitest/config'
import plugin from '@vitejs/plugin-react'
import fs from 'fs'
import path from 'path'
import child_process from 'child_process'
import { env } from 'process'

export default defineConfig(({ command }) => {
    // Only generate certificates on dev mode 
    let httpsConfig = undefined

    if (command === 'serve') {
        const baseFolder =
            env.APPDATA && env.APPDATA !== ''
                ? `${env.APPDATA}/ASP.NET/https`
                : `${env.HOME}/.aspnet/https`

        const certificateName = '3dmanager-app.client'
        const certFilePath = path.join(baseFolder, `${certificateName}.pem`)
        const keyFilePath = path.join(baseFolder, `${certificateName}.key`)

        if (!fs.existsSync(baseFolder)) {
            fs.mkdirSync(baseFolder, { recursive: true })
        }

        if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
            const result = child_process.spawnSync(
                'dotnet',
                [
                    'dev-certs',
                    'https',
                    '--export-path',
                    certFilePath,
                    '--format',
                    'Pem',
                    '--no-password'
                ],
                { stdio: 'inherit' }
            )

            if (result.status !== 0) {
                throw new Error('Could not create certificate.')
            }
        }

        httpsConfig = {
            key: fs.readFileSync(keyFilePath),
            cert: fs.readFileSync(certFilePath)
        }
    }

    const isCI = process.env.VITE_CI === 'true'
    const target = isCI
        ? 'http://localhost:5000'
        : env.ASPNETCORE_HTTPS_PORT
            ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
            : 'https://localhost:443'

    return {
        plugins: [plugin()],
        resolve: {
            alias: {
                '@': fileURLToPath(new URL('./src', import.meta.url))
            }
        },
        server: isCI
            ? {
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
                        changeOrigin: true
                    }
                },
                port: 3000,
                https: httpsConfig 
            }
    }
})
