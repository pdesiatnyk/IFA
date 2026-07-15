import { describe, expect, it } from 'vitest';
import { check, parseUdi } from '../src/parser.js';
import { IfaUdiFormatError } from '../src/types.js';
import { loadFixtures } from './fixtures.js';

const fixtures = loadFixtures();

describe('check', () => {
  for (const fixture of fixtures.barcodeFixtures) {
    it(`returns ${fixture.expectedValid} for ${fixture.name}`, () => {
      expect(check(fixture.input)).toBe(fixture.expectedValid);
    });
  }
});

describe('parseUdi', () => {
  for (const fixture of fixtures.barcodeFixtures) {
    if (!fixture.expectedValid) {
      it(`throws IfaUdiFormatError for ${fixture.name}`, () => {
        expect(() => parseUdi(fixture.input)).toThrow(IfaUdiFormatError);
      });
      continue;
    }

    it(`parses ${fixture.name} into the expected structure`, () => {
      const actual = parseUdi(fixture.input);
      const expected = fixture.expected!;

      expect(actual.udiDi.raw).toBe(expected.udiDi.raw);
      expect(actual.udiDi.scheme).toBe(expected.udiDi.scheme);
      expect(actual.udiDi.praCode).toBe(expected.udiDi.praCode);
      expect(actual.udiDi.pzn).toBe(expected.udiDi.pzn);
      expect(actual.udiDi.cin).toBe(expected.udiDi.cin);
      expect(actual.udiDi.itemReference).toBe(expected.udiDi.itemReference);
      expect(actual.udiDi.packagingLevelIndex).toBe(expected.udiDi.packagingLevelIndex);
      expect(actual.udiDi.deviceGroupCode).toBe(expected.udiDi.deviceGroupCode);
      expect(actual.udiDi.nationalCode).toBe(expected.udiDi.nationalCode);
      expect(actual.udiDi.checkDigits).toBe(expected.udiDi.checkDigits);

      expect(actual.udiPi.lot).toBe(expected.udiPi.lot);
      expect(actual.udiPi.expiryDate).toBe(expected.udiPi.expiryDate);
      expect(actual.udiPi.manufacturingDate).toBe(expected.udiPi.manufacturingDate);
      expect(actual.udiPi.serialNumber).toBe(expected.udiPi.serialNumber);
      expect(actual.udiPi.quantity).toBe(expected.udiPi.quantity);
      expect(actual.udiPi.price).toBe(expected.udiPi.price);
      expect(actual.udiPi.url).toBe(expected.udiPi.url);
      expect(actual.udiPi.additionalGtins).toEqual(expected.udiPi.additionalGtins);
    });
  }
});
