export const state = {
  items: [
    { id: 1, title: 'نصب لینوکس اوبونتو', createdAt: 'یک سال پیش', updatedAt: 'دیروز' },
    { id: 2, title: 'دستورات مدیریتی در لینوکس', createdAt: 'یک سال پیش', updatedAt: 'دیروز' },
  ],
  details: {
    id: 1,
    title: 'نصب لینوکس اوبونتو',
    createdAt: 'یک سال پیش',
    updatedAt: 'دیروز',
    tags: [],
    category: { id: 1, title: 'سیستم عامل لینوکس' },
    contents: `## نصب از طریق فایل ISO`
  }
}

export const getters = {
  items: state => state.items,
  details: state => state.details,
}