import { describe, it, expect } from "vitest";
import { convertHumanTimeToSeconds, parseGcodeData } from "../../models/print/GCodePatterns";

describe("convertHumanTimeToSeconds", () => {

    it("ConversionHoursMinutesAndSecons", () => {
        const result = convertHumanTimeToSeconds("8h 49m 43s");
        expect(result).toBe(8 * 3600 + 49 * 60 + 43);
    });

    it("ConversionMinutesAndSecons", () => {
        const result = convertHumanTimeToSeconds("13m 21s");
        expect(result).toBe(13 * 60 + 21);
    });

    it("ConversionSecons", () => {
        const result = convertHumanTimeToSeconds("45s");
        expect(result).toBe(45);
    });

    it("Returns0WhenNoCoincidence", () => {
        const result = convertHumanTimeToSeconds("nada");
        expect(result).toBe(0);
    });
});

describe("convertHumanTimeToSeconds", () => {

    it("ConversionHoursMinutesAndSecons", () => {
        const result = convertHumanTimeToSeconds("8h 49m 43s");
        expect(result).toBe(8 * 3600 + 49 * 60 + 43);
    });

    it("ConversionMinutesAndSecons", () => {
        const result = convertHumanTimeToSeconds("13m 21s");
        expect(result).toBe(13 * 60 + 21);
    });

    it("ConversionSecons", () => {
        const result = convertHumanTimeToSeconds("45s");
        expect(result).toBe(45);
    });

    it("Returns0WhenNoCoincidence", () => {
        const result = convertHumanTimeToSeconds("nada");
        expect(result).toBe(0);
    });
});

describe("parseGcodeData", () => {

    it("SlicerFormat1Reader", () => {
        const gcode = `
        ;TIME:20053.1
        ;Filament used:19.3763m
        `;

        const result = parseGcodeData(gcode);

        expect(result.time).toBe(20053.1);
        expect(result.filament).toBe(19.3763);
    });

    it("SlicerFormat2Reader", () => {
        const gcode = `
        ; estimated printing time (normal mode) = 8h 49m 43s
        ; filament used [mm] = 26346.67
        `;

        const result = parseGcodeData(gcode);
        console.log(result);
        expect(result.time).toBe(8 * 3600 + 49 * 60 + 43);
        expect(result.filament).toBeCloseTo(26.34667, 5); 
    });

    it("SlicerFormatPartialValuesRead", () => {
        const gcode = `
        ; estimated printing time (normal mode) = 13m 21s
        `;

        const result = parseGcodeData(gcode);

        expect(result.time).toBe(13 * 60 + 21);
        expect(result.filament).toBe(0);
    });

    it("Return0WhenNoCoincidence", () => {
        const gcode = `; filament used [cm3] = 2.94
        ; estimated first layer printing time (normal mode) = 13s
        `;

        const result = parseGcodeData(gcode);

        expect(result.time).toBe(0);
        expect(result.filament).toBe(0);
    });
});