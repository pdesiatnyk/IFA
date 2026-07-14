import { describe, expect, it } from 'vitest';
import { mod97, mod11Pzn } from '../src/checkDigits.js';
import { loadFixtures } from './fixtures.js';

const fixtures = loadFixtures();

describe('mod97', () => {
  for (const fixture of fixtures.checkDigitFixtures.mod97) {
    it(`computes ${fixture.expected} for '${fixture.input}' (${fixture.note ?? ''})`, () => {
      expect(mod97(fixture.input)).toBe(fixture.expected);
    });
  }
});

describe('mod11Pzn', () => {
  for (const fixture of fixtures.checkDigitFixtures.mod11Pzn) {
    it(`computes ${fixture.expected} for '${fixture.input}' (${fixture.note ?? ''})`, () => {
      expect(mod11Pzn(fixture.input)).toBe(fixture.expected);
    });
  }
});
