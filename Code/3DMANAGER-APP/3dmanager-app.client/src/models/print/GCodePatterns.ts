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