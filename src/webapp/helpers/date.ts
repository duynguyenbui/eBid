export function calEndingTimeBasedOnDuration(
  createDate: Date,
  durationInDays: number
) {
  const durationInMilliseconds = durationInDays * 24 * 60 * 60 * 1000;
  return new Date(createDate.getTime() + durationInMilliseconds);
}

function convertToDays(endingTime: string, createTime: string): number {
  const endTime: Date = new Date(endingTime);
  const startTime: Date = new Date(createTime);

  const timeDifference: number = endTime.getTime() - startTime.getTime();

  const daysDifference: number = timeDifference / (1000 * 60 * 60 * 24);

  return Math.floor(daysDifference);
}
