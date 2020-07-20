
export const toQueryString = (json, prefix) => {
  if (!json) return '';
  return Object.keys(json)
    .map(
      key =>
        (prefix ? prefix + '.' : '') +
        encodeURIComponent(key) +
        '=' +
        (json[key] != null ? encodeURIComponent(json[key]) : '')
    )
    .join('&');
}
