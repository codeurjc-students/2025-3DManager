export const timePatterns = [
    {
        regex: /^;TIME:(.*)$/i,
        parse: (raw: string) => Number(raw.trim()) || 0
    },

    {
        regex: /^; estimated printing time \(normal mode\) = (.*)$/i,
        parse: (raw: string) => convertHumanTimeToSeconds(raw)
    },
];

export const filamentPatterns = [
    {
        regex: /^;Filament used:(.*)$/i,
        parse: (raw: string) =>
            Number(raw.replace("m", "").trim()) || 0
    },

    {
        regex: /^; filament used \[mm\] = (.*)$/i,
        parse: (raw: string) =>
            (Number(raw.trim()) || 0) / 1000 
    }
];

export function convertHumanTimeToSeconds(text: string): number {
    let total = 0;

    const h = /(\d+)h/.exec(text);
    const m = /(\d+)m/.exec(text);
    const s = /(\d+)s/.exec(text);

    if (h) total += Number(h[1]) * 3600;
    if (m) total += Number(m[1]) * 60;
    if (s) total += Number(s[1]);

    return total;
}

export function parseGcodeData(text: string) {
    const lines = text.split("\n");

    let timeValue = 0;
    let filamentValue = 0;

    for (let line of lines) {
        const cleanLine = line.trim();
        for (const pattern of timePatterns) {
            const match = pattern.regex.exec(cleanLine);
            if (match) {
                timeValue = pattern.parse(match[1]);
            }
        }

        for (const pattern of filamentPatterns) {
            const match = pattern.regex.exec(cleanLine);
            if (match) {
                filamentValue = pattern.parse(match[1]);
                break;
            }
        }
    }

    return {
        time: timeValue,
        filament: filamentValue
    };
}