import { HttpErrorResponse } from '@angular/common/http';

/**
 * Turns an API failure into one sentence for the user. The API returns
 * ProblemDetails for handled errors and ValidationProblemDetails for model
 * validation failures.
 */
export function describeHttpError(error: unknown): string {
  if (!(error instanceof HttpErrorResponse)) {
    return 'Something went wrong. Please try again.';
  }

  const body = error.error as { detail?: string; errors?: Record<string, string[]> } | null;

  // Status 0 is a network failure. A 5xx without ProblemDetails is the dev
  // proxy reporting that the API itself is not listening.
  if (error.status === 0 || (error.status >= 500 && !body?.detail)) {
    return "Can't reach the KnowHub server. Check that the API is running.";
  }

  const firstValidationMessage = body?.errors ? Object.values(body.errors).flat()[0] : undefined;

  return firstValidationMessage ?? body?.detail ?? 'Something went wrong. Please try again.';
}
