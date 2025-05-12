namespace Common.Constants;

/// <summary>
/// Constants for application response messages.
/// </summary>
internal static class AppCodes
{
    // ✅ General success and error messages
    public const string SUCCESS = "The operation was completed successfully.";
    public const string FAILURE = "The operation could not be completed. Please try again.";
    public const string ERROR = "An unexpected error occurred on the server.";
    public const string INTERNAL_SERVER_ERROR =
        "A server-side error occurred. Please try again later.";

    // ✅ Authentication & Authorization
    public const string UNAUTHORIZED = "You are not authorized to perform this action.";
    public const string FORBIDDEN = "Access to this resource is denied.";
    public const string ACCESS_DENIED = "You do not have permission to access this resource.";
    public const string TOKEN_EXPIRED = "Your session has expired. Please log in again.";
    public const string INVALID_TOKEN = "The provided token is invalid or has expired.";
    public const string LOGIN_SUCCESS = "Login was successful.";
    public const string LOGIN_FAILED = "Login failed. Please check your username and password.";
    public const string LOGOUT_SUCCESS = "You have been logged out successfully.";
    public const string SESSION_EXPIRED = "Your session has expired. Please log in again.";

    // ✅ Request validation and input errors
    public const string BAD_REQUEST = "The request was invalid or cannot be processed.";
    public const string VALIDATION_FAILED =
        "Validation failed. Please review your input and try again.";
    public const string MISSING_REQUIRED_FIELDS = "Some required fields are missing.";
    public const string INVALID_INPUT = "The input data is invalid.";
    public const string INVALID_PARAMETERS = "One or more request parameters are invalid.";

    // ✅ CRUD operations
    public const string CREATED = "The resource was successfully created.";
    public const string UPDATED = "The resource was successfully updated.";
    public const string DELETED = "The resource was successfully deleted.";
    public const string CREATE_FAILED = "Failed to create the resource.";
    public const string UPDATE_FAILED = "Failed to update the resource.";
    public const string DELETE_FAILED = "Failed to delete the resource.";

    // ✅ Resource and data-related
    public const string NOT_FOUND = "The requested resource could not be found.";
    public const string ALREADY_EXISTS = "A resource with the same data already exists.";
    public const string DUPLICATE_ENTRY = "Duplicate entry detected.";
    public const string CONFLICT = "A conflict occurred. The operation could not be completed.";
    public const string RESOURCE_LOCKED = "The resource is currently locked or being used.";
    public const string RESOURCE_BUSY = "The system is busy. Please try again shortly.";
    public const string EMPTY_RESULT = "No data was found for the given criteria.";

    // ✅ File and upload handling
    public const string FILE_UPLOAD_SUCCESS = "The file was uploaded successfully.";
    public const string FILE_UPLOAD_FAILED = "The file upload failed.";
    public const string FILE_TOO_LARGE = "The uploaded file exceeds the allowed size limit.";
    public const string INVALID_FILE_FORMAT = "The uploaded file format is not supported.";

    // ✅ Rate limiting, throttling, quota
    public const string TOO_MANY_REQUESTS = "Too many requests. Please slow down.";
    public const string RATE_LIMIT_EXCEEDED = "Rate limit exceeded. Try again later.";
    public const string QUOTA_EXCEEDED = "You have exceeded your usage quota.";

    // ✅ Email and notification
    public const string EMAIL_SENT = "The email was sent successfully.";
    public const string EMAIL_FAILED = "Failed to send the email.";
    public const string VERIFICATION_EMAIL_SENT =
        "A verification email has been sent to your address.";

    // ✅ Payment and transaction
    public const string PAYMENT_SUCCESS = "The payment was processed successfully.";
    public const string PAYMENT_FAILED = "The payment failed. Please try again.";
    public const string PAYMENT_PENDING = "The payment is currently being processed.";
    public const string INSUFFICIENT_FUNDS = "Insufficient funds to complete the transaction.";

    // ✅ System state and maintenance
    public const string SYSTEM_UNDER_MAINTENANCE =
        "The system is under maintenance. Please try again later.";
    public const string FEATURE_DISABLED = "This feature is currently disabled.";

    // ✅ User management
    public const string USER_CREATED = "User account was successfully created.";
    public const string USER_UPDATED = "User profile was successfully updated.";
    public const string USER_DELETED = "User account was successfully deleted.";
    public const string USER_DEACTIVATED = "User account was successfully deactivated.";
    public const string USER_REACTIVATED = "User account was successfully reactivated.";
    public const string USER_NOT_FOUND = "The requested user could not be found.";
    public const string USER_ALREADY_EXISTS = "A user with this username or email already exists.";
    public const string USERNAME_TAKEN =
        "This username is already taken. Please choose another one.";
    public const string EMAIL_ALREADY_REGISTERED = "This email address is already registered.";

    // ✅ User authentication & password management
    public const string PASSWORD_RESET_REQUESTED =
        "Password reset instructions have been sent to your email.";
    public const string PASSWORD_RESET_SUCCESS = "Your password has been reset successfully.";
    public const string PASSWORD_RESET_FAILED = "Failed to reset your password. Please try again.";
    public const string PASSWORD_CHANGED = "Your password has been changed successfully.";
    public const string PASSWORD_MISMATCH =
        "The new password and confirmation password do not match.";
    public const string PASSWORD_TOO_WEAK = "Password does not meet the strength requirements.";
    public const string PASSWORD_RECENTLY_USED =
        "This password was recently used. Please choose a different one.";
    public const string ACCOUNT_LOCKED =
        "Your account has been locked due to multiple failed login attempts.";
    public const string ACCOUNT_UNLOCKED = "Your account has been unlocked successfully.";

    // ✅ User verification & 2FA
    public const string EMAIL_VERIFICATION_REQUIRED =
        "Please verify your email address to continue.";
    public const string EMAIL_VERIFIED = "Your email address has been verified successfully.";
    public const string PHONE_VERIFICATION_REQUIRED =
        "Please verify your phone number to continue.";
    public const string PHONE_VERIFIED = "Your phone number has been verified successfully.";
    public const string MFA_ENABLED =
        "Multi-factor authentication has been enabled for your account.";
    public const string MFA_DISABLED =
        "Multi-factor authentication has been disabled for your account.";
    public const string MFA_REQUIRED = "Multi-factor authentication is required for this action.";
    public const string MFA_CODE_SENT = "A verification code has been sent to your device.";
    public const string MFA_CODE_INVALID = "The verification code is invalid or has expired.";

    // ✅ User roles & permissions
    public const string ROLE_ASSIGNED = "The role has been successfully assigned to the user.";
    public const string ROLE_REMOVED = "The role has been successfully removed from the user.";
    public const string PERMISSION_GRANTED =
        "The permission has been successfully granted to the user.";
    public const string PERMISSION_REVOKED =
        "The permission has been successfully revoked from the user.";
    public const string INSUFFICIENT_PERMISSIONS =
        "You have insufficient permissions to perform this action.";

    // ✅ User profile & preferences
    public const string PROFILE_UPDATED = "Your profile has been updated successfully.";
    public const string AVATAR_UPDATED = "Your profile picture has been updated successfully.";
    public const string PREFERENCES_UPDATED = "Your preferences have been updated successfully.";
    public const string NOTIFICATION_SETTINGS_UPDATED =
        "Your notification settings have been updated successfully.";
    public const string PRIVACY_SETTINGS_UPDATED =
        "Your privacy settings have been updated successfully.";

    // ✅ User session management
    public const string SESSION_CREATED = "A new session has been created successfully.";
    public const string SESSION_TERMINATED = "The session has been terminated successfully.";
    public const string ALL_SESSIONS_TERMINATED =
        "All your sessions have been terminated successfully.";
    public const string ACTIVE_SESSIONS_LISTED = "Active sessions retrieved successfully.";
    public const string SESSION_NOT_FOUND = "The specified session could not be found.";

    // ✅ Social & Third-party authentication
    public const string SOCIAL_AUTH_SUCCESS = "Social authentication was successful.";
    public const string SOCIAL_AUTH_FAILED = "Social authentication failed. Please try again.";
    public const string ACCOUNT_LINKED = "Your account has been linked successfully.";
    public const string ACCOUNT_UNLINKED = "Your account has been unlinked successfully.";
    public const string SOCIAL_ACCOUNT_ALREADY_LINKED =
        "This social account is already linked to another user.";
}
