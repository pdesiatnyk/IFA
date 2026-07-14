/**
 * Shared validation rules used by both the parser and the builder, so parse-side and
 * build-side rules cannot drift apart. See documentation/IFA_UDI_Parser_Analysis.md sections 2-3.
 */

export const FORBIDDEN_LOT_SN_CHARS = /[\x00-\x1F\x7F-\xFF#$@[\\\]^`{|}~]/;
export const ITEM_REFERENCE_CHARSET = /^[0-9A-Z.-]+$/;
export const ALPHANUMERIC_UPPER_CHARSET = /^[0-9A-Z]+$/;
